using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WMate1.Models;
using WMate1.ViewModels;
using UserCredential = WMate1.Models.UserCredential;

namespace WMate1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult About()
        {

            return View();
        }
        public ActionResult Contact()
        {

            return View();
        }
        public ActionResult Trips()
        {
            TripsViewModel model = new TripsViewModel();
            DataContext db = new DataContext();
            
            List<Entry> entrieses = db.Entries.OrderByDescending(x => x.CreatedDate).ToList();

            model.EntryList = entrieses;
           
            return View(model);
        }
        public ActionResult Entries(int id)
        {
            //entry sayfasını bitir..
            DataContext db = new DataContext();
            CommentViewModel model = new CommentViewModel();
            
            List<Comment> commentses = db.Comments.Where(x => x.EntryId == id).ToList();
            Entry entry = db.Entries.FirstOrDefault(x => x.ID == id);
            
            model.Entry = entry;
            model.CommentsesList = commentses;

            return View(model);
        }
        [HttpPost]
        public ActionResult Entries(CommentViewModel model)
        {
            DataContext db = new DataContext();
            UserCredential user = new UserCredential();
            user = Session["Login"] as UserCredential;
            Comment comment = new Comment();
            comment.Description = model.Comment.Description;
            comment.UserId = user.ID;
            comment.EntryId = model.Entry.ID;
            comment.UploadDate = DateTime.Now;

            if (string.IsNullOrEmpty(model.Comment.Description))
            {
                ModelState.AddModelError("", "Boş bir yorum ekleyemezsiniz!");
            }
            else
            {
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            return RedirectToAction("Entries", new { id = model.Entry.ID });
            
        }
        
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            //Session'a bilgi saklama
            DataContext db = new DataContext();
            UserCredential user = db.UserCredentials.FirstOrDefault(x => x.UserName == model.UserName);
            bool flag = true;
      
            if (ModelState.IsValid)
            {

                if (user == null)
                {
                    ModelState.AddModelError("", "Wrong username or password.");
                    flag = false;
                    return View(model);
                }
                
                if (Crypto.VerifyHashedPassword(user.UserPass, model.Password ))
                { 
                    flag = true;
                }
                else
                {
                    ModelState.AddModelError("", "Wrong username or password.");
                    flag = false;
                }
                if (user.IsActive == false)
                {
                    ModelState.AddModelError("", "Acoount isn't activated.");
                    flag = false;
                }
                if (flag == false)
                {
                    return View(model);
                }
            }
            Session["Login"] = user;

            return RedirectToAction("Index");
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session["Login"] = null;

            return RedirectToAction("Index");
        }
        
        public ActionResult ShowProfile(LoginViewModel model)
        {
            DataContext db = new DataContext();
            UserCredential user = db.UserCredentials.FirstOrDefault(x => x.UserName == model.UserName);
            user = Session["Login"] as UserCredential;
          
            return View(user);
        }
        private void SendVerificationMail(UserCredential model)
        {
            DataContext db = new DataContext();
            UserCredential user = db.UserCredentials.FirstOrDefault(x => x.UserName == model.UserName || x.UserMail == model.UserMail);
            Guid guid = (Guid)user.ActiveGuid;
            var senderEmail = new MailAddress("waymatecyprus@gmail.com", "");
            var receivereEmail = new MailAddress(user.UserMail, "Receiver");
            var password = "wm102019";
            var body = "Hi "+user.UserName+",\n"+"\n Welcome to WayMate!\n\n Thanks for joining us. There is only one more step left! Please activate your account using activation link below.\n\n" +
                " Activation link: https://localhost:44365/Home/UserActivation/" + guid.ToString();
            var sub = "Welcome to Way Mate";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("waymatecyprus@gmail.com", password),
            };
            smtp.Send(senderEmail.ToString(), receivereEmail.ToString(), sub, body);
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
          
            DataContext db = new DataContext();
            UserCredential regUser = db.UserCredentials.FirstOrDefault(x => x.UserName == model.Username || x.UserMail == model.Email);
            bool flag = true;

            if (ModelState.IsValid)
            {
                if (regUser == null)
                {
                    UserCredential user = new UserCredential();
                    user.UserName = model.Username;
                    user.UserMail = model.Email;
                    string cryptedPass = Crypto.HashPassword(model.Password);
                    user.UserPass = cryptedPass;
                    user.IsAdmin = false;
                    user.IsActive = false;
                    user.IsArchieved = false;
                    user.IsDriver = false;
                    user.ProfileImage = @"..\autoroad\images\profile image.png";
                    user.ActiveGuid = Guid.NewGuid();
                    db.UserCredentials.Add(user);
                    db.SaveChanges();
                    Session["Login"] = user.UserName;
                    SendVerificationMail(user);

                    return RedirectToAction("Index");
                }
                if (model.Username == regUser.UserName)
                {
                    ModelState.AddModelError("", "This username or e-mail address is already registred.");
                    flag = false;
                }

                else if (model.Email == regUser.UserMail)
                {
                    ModelState.AddModelError("", "This username or e-mail address is already registred.");
                    flag = false;
                }

                if (flag == false)
                    return View(model);
            }

            return View(model);
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult RegisterOk()
        {
            return View();
        }
        public ActionResult UserActivation(Guid? id)
        {
            DataContext db = new DataContext();
            UserCredential user = db.UserCredentials.FirstOrDefault(x => x.ActiveGuid == id);

            if (id == user.ActiveGuid)
            {
                user.IsActive = true;
                db.SaveChanges();
            }
           
            return View();
        }
        public ActionResult EditProfile(int id)
        {
            DataContext db = new DataContext();
            UserCredential user = db.UserCredentials.FirstOrDefault(x => x.ID == id);
            
            return View(user);
        }

        [HttpPost]
        public ActionResult EditProfile(UserCredential model, HttpPostedFileBase file)
        {
           DataContext db = new DataContext();
           UserCredential user = db.UserCredentials.FirstOrDefault(x => x.ID == model.ID);
           model = user;

           if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var size = file.ContentLength;          //Checking image size
                    var type = file.ContentType;
                    if (file.ContentLength > 2097152)
                    {
                        ModelState.AddModelError("", "Image size cannot be bigger than 2 mb.");
                        return View(model);
                    }
                    if (file.ContentType.ToString() != "image/png" || file.ContentType.ToString() != "image/jpeg")
                    {
                        ModelState.AddModelError("", "Wrong Format");
                        return View(model);
                    }

                    //Get width and height
                    System.IO.Stream stream = file.InputStream;
                    Image img = Image.FromStream(stream);
                    Image profImage = ResizeImage(img);
                    //saving image name
                    string fileName = $"user_{model.ID}.{file.ContentType.Split('/')[1]}";
                    profImage.Save(Server.MapPath($"~/images/{fileName}"), ImageFormat.Jpeg);
                    model.ProfileImage = fileName;
                }
                // Construct the viewmodel
                user.UserName = model.UserName;
                string cryptedPass = Crypto.HashPassword(model.UserPass);
                user.UserPass = cryptedPass;
                user.ProfileImage = model.ProfileImage;
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError("", "Wrong Format");
                            return View(model);
                        }
                    }
                }
            }

            return RedirectToAction("ShowProfile");
        }

        public Image ResizeImage(Image img)
        {
            int height = img.Height;
            int width = img.Width;
            double xRatio = (double)img.Width / 128;
            double yRatio = (double)img.Height / 128;
            double ratio = Math.Max(xRatio, yRatio);
            int nnx = 128;
            int nny = 128;
            Bitmap cpy = new Bitmap(nnx, nny, PixelFormat.Format32bppArgb);
            using (Graphics gr = Graphics.FromImage(cpy))
            {

                gr.Clear(Color.White);
                // This is said to give best quality when resizing images
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;

                gr.DrawImage(img,
                    new Rectangle(0, 0, nnx, nny),
                    new Rectangle(0, 0, img.Width, img.Height),
                    GraphicsUnit.Pixel);

            }
            return cpy;
        }
        public ActionResult PostEntry()
        {
            DataContext db = new DataContext();
            return View();
        }
        [HttpPost]
        public ActionResult PostEntry(EntriesViewModel model)
        {
            DataContext db = new DataContext();
            UserCredential user = new UserCredential();
            user = Session["Login"] as UserCredential;
            Entry entryUserId = new Entry();
            Entry entry = new Entry();
            var desc = model.Entries.Description; //This is for edit text that we take from user for a better view
           
            user = Session["Login"] as UserCredential;
            if (ModelState.IsValid)
            {
                entry.Title = model.Entries.Title;
                entry.Description = desc;
                entry.DropOffLoc = model.Entries.DropOffLoc;
                entry.PickUpLoc = model.Entries.PickUpLoc;
                entry.CreatedDate = DateTime.Now;
                entry.UserId = user.ID;
                db.Entries.Add(entry);

                db.SaveChanges();
            }
            return View();
        }

        [ValidateInput(false)]
        public JsonResult DenemeEntry(EntriesViewModel model)
        {
           
            DataContext db = new DataContext();
           
            return Json("200");
        }
    }

}
