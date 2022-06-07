using Sqlserver.maid.Services.Extension;
using stdole;
using System.Drawing;
using System.Windows.Forms;

namespace Sqlserver.maid.Services
{
    public class IconConverter : AxHost
    {
        public IconConverter() : base(string.Empty)
        {
        }

        public static IPictureDisp GetPictureDispFromImage(Image image)
        {
            return GetIPictureDispFromPicture(image).As<IPictureDisp>();
        }
    }
}