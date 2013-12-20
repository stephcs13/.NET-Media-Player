﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WindowsMedia.classes
{
    [ValueConversion(typeof(TimeSpan), typeof(String))]
    public class DurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan duration = (TimeSpan)value;
            return String.Format("{0:d2}:{1:d2}:{2:d2}", duration.Hours, duration.Minutes, duration.Seconds);
        }

        public object ConvertBack(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            String str = (String)value;
            TimeSpan result;
            if (TimeSpan.TryParse(str, out result))
                return result;
            return new TimeSpan();
        }
    }

    public class MusicTitle
    {
        public MusicArtist Artist { get; private set; }
        public MusicAlbum Album { get; private set; }
        public string Genre { get; private set; }
        public uint Year { get; private set; }
        public uint TrackNumber { get; private set; }
        public string Title { get; private set; }
        public string Composer { get; private set; }
        public TimeSpan Duration { get; private set; }
        public BitmapImage Image { get; private set; }
        public String Path { get; private set; }

        public MusicTitle(MusicArtist artist, MusicAlbum album, TagLib.File tags, String file)
        {
            Path = file;
            Artist = artist;
            Album = album;
            Genre = tags.Tag.FirstGenre;
            Year = tags.Tag.Year;
            TrackNumber = tags.Tag.Track;
            Title = tags.Tag.Title;
            Composer = tags.Tag.FirstComposer;
            Duration = tags.Properties.Duration;
            if (tags.Tag.Pictures.Length > 0)
            {
                Image = new BitmapImage();
                Image.BeginInit();
                Image.StreamSource = new MemoryStream(tags.Tag.Pictures[0].Data.Data);
                Image.EndInit();
            }
            else
            {
                Image = new BitmapImage(new Uri("../assets/defaultalbumart.png", UriKind.Relative));
            }
            Image.Freeze();
        }
    }
}
