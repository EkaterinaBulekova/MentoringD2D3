using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameOfLife
{
    class AdService
    {
        private Advertise[] advertises;

        public AdService()
        {
            InitAdvertises();
        }

        public Advertise this[int index]
        {
            get => advertises[index];
        }

        public int Count
        {
            get => advertises.Length;
        }

        private void InitAdvertises()
        {
            advertises = new Advertise[3];
            advertises[0] = new Advertise
            {
                Image = new ImageBrush() { ImageSource = new BitmapImage(new Uri("ad1.jpg", UriKind.Relative)) },
                Link = "http://example.com"
            };
            advertises[1] = new Advertise
            {
                Image = new ImageBrush() { ImageSource = new BitmapImage(new Uri("ad2.jpg", UriKind.Relative)) },
                Link = "http://example.com"
            };
            advertises[2] = new Advertise
            {
                Image = new ImageBrush() { ImageSource = new BitmapImage(new Uri("ad3.jpg", UriKind.Relative)) },
                Link = "http://example.com"
            };
        }

    }
}
