using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MainClass
{
    public class ImageGUIDReference
    {
        public String DefaultImage { get; set; }
        public String BasePath { get; set; }
        public String GUID { get; set; }
        public ImageGUIDReference(String basePath, String defautImage)
        {
            this.BasePath = basePath;
            this.DefaultImage = defautImage;
            GUID = "";
        }

        public String GetURL()
        {
            String url;
            if (String.IsNullOrEmpty(GUID))
                url = BasePath + DefaultImage;
            else
                url = BasePath + GUID + ".png";
            return url;
        }

        public String GetURI()
        {
            String uri;
            if (String.IsNullOrEmpty(GUID))
                uri = "";
            else
                uri = BasePath + GUID + ".png";
            return uri;
        }

        public String GetImageURL(String GUID)
        {
            String url;
            if (String.IsNullOrEmpty(GUID))
                url = BasePath + DefaultImage;
            else
                url = BasePath + GUID + ".png";
            return url;
        }

        public String UpLoadImage(HttpRequestBase Request, String PreviousGUID)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    GUID = PreviousGUID;
                    if (!String.IsNullOrEmpty(GUID))
                    {
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath(GetURI()));
                    }
                    GUID = Guid.NewGuid().ToString();
                    file.SaveAs(HttpContext.Current.Server.MapPath(GetURI()));
                    return GUID;
                }
            }
            return PreviousGUID;
        }

        public void Remove(String GUID)
        {
            if (!String.IsNullOrEmpty(GUID))
            {
                this.GUID = GUID;
                System.IO.File.Delete(HttpContext.Current.Server.MapPath(GetURI()));
            }
        }
    }

    public class Cour
    {
        public long Id { get; set; }

        [Display(Name = "Code du cours")]
        [StringLength(50), Required]
        [RegularExpression(@"^((?!^Name$)[-a-zA-Z0-9 àâäçèêëéìîïòôöùûüÿñÀÂÄÇÈÊËÉÌÎÏÒÔÖÙÛÜ_'])+$", ErrorMessage = "Caractères illégaux.")]
        public String CodeCours { get; set; }

        [Display(Name = "Nom du cours")]
        [StringLength(50), Required]
        public String NomCours { get; set; }

        [Display(Name = "Ponderation")]
        [StringLength(50), Required]
        public String Ponderation { get; set; }

        public Cour()
        {
            CodeCours = "";
            NomCours = "";
            Ponderation = "";
        }

    }

    public class Cours : SqlExpressUtilities.SqlExpressWrapper
    {
        public Cour cour { get; set; }

        public Cours(object cs)
            : base(cs)
        {
            cour = new Cour();
        }

        public Cours() { cour = new Cour(); }

        public List<Cour> ToList()
        {
            List<object> list = this.RecordsList();
            List<MainClass.Cour> cours_list = new List<Cour>();
            foreach (Cour cour in list)
            {
                cours_list.Add(cour);
            }
            return cours_list;
        }

        public override void DeleteRecordByID(String ID)
        {
            if (this.SelectByID(ID))
            {
                base.DeleteRecordByID(ID);
            }
        }
    }

    public class Programme
    {
        public long Id { get; set; }

        [Display(Name = "Code du programme")]
        [StringLength(50), Required]
        public String CodeProgramme { get; set; }

        [Display(Name = "Nom du programme")]
        [StringLength(50), Required]
        public String NomProgramme { get; set; }

        [Display(Name = "Photo du programme")]
        [StringLength(50), Required]
        public String PhotoProgramme { get; set; }

        private ImageGUIDReference ImageReference;

        public Programme()
        {
            CodeProgramme = "";
            NomProgramme = "";
            PhotoProgramme = "";
            ImageReference = new ImageGUIDReference(@"/Images/Programmes/", @"UnknownPoster.png");
        }

        public String GetPosterURL()
        {
            return ImageReference.GetImageURL(PhotoProgramme);
        }

        public void UpLoadPoster(HttpRequestBase Request)
        {
            PhotoProgramme = ImageReference.UpLoadImage(Request, PhotoProgramme);
        }

        public void RemovePoster()
        {
            ImageReference.Remove(PhotoProgramme);
        }
    }
    public class Programmes : SqlExpressUtilities.SqlExpressWrapper
    {
        public Programme programme { get; set; }
        public Programmes(object cs)
            : base(cs)
        {
            programme = new Programme();
        }
        public Programmes() { programme = new Programme(); }

        public List<Programme> ToList()
        {
            List<object> list = this.RecordsList();
            List<MainClass.Programme> programmes_list = new List<Programme>();
            foreach (Programme programme in list)
            {
                programmes_list.Add(programme);
            }
            return programmes_list;
        }

        public override void DeleteRecordByID(String ID)
        {
            if (this.SelectByID(ID))
            {
                this.programme.RemovePoster();
                base.DeleteRecordByID(ID);
            }
        }
    }


}