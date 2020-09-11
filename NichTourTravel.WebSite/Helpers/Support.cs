using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NichTourTravel.WebSite.Helpers
{
    public static class Support
    {
        public enum MessageType { warning, success }
        public static string ShowMessage(MessageType type, string message)
        {
            return $@"
            <div class='alert alert-{type.ToString()} alert-dismissible fade show' role='alert'>
              {message}
              <button type='button' class='close' data-dismiss='alert' aria-label='Close'>
                <span aria-hidden='true'>&times;</span>
              </button>
            </div>";
        }
        public static int GetID()
        {
            var date = DateTime.UtcNow;
            var year = date.Year;
            var day = date.Day;
            var month = date.Month;
            var hour = date.Hour;
            var min = date.Minute;
            var sec = date.Second;
            var mill = date.Millisecond;

            var newId = year + month + day + hour + min + sec + mill;
            return newId;
        }
        public static string TwLink(string url, string text)
        {
            if (text.Contains("'"))
            {
                text = text.Replace("'", "&rsquo;");
            }

            return $"<a class='share' aria-hidden='true' id='tw' href='https://twitter.com/intent/tweet?text={text}&url={url}'><i class='fab fa-twitter-square'></i></a>";
        }
        public static string FbLink(string url)
        {
            return $"<a id='fb' target='_blank' aria-hidden='true' href='http://www.facebook.com/sharer/sharer.php?u={url}' class='fb-xfbml-parse-ignore'><i class='fab fa-facebook-square'></i></a>";
        }
        public static string WpLink(string url)
        {
            return $"<a href='https://api.whatsapp.com/send?text={url}' id='wp' data-action='share/whatsapp/share' aria-hidden='true'><i class='fab fa-whatsapp'></i></a>";
        }
        /// <summary>
        /// gets relative time
        /// </summary>
        /// <param name="date">date in utc time</param>
        /// <returns></returns>
        public static string GetRelativetime(this DateTime date)
        {
            var timeSpan = DateTime.UtcNow.AddHours(3).Subtract(date.AddHours(3));

            string result;
            if (timeSpan <= TimeSpan.FromSeconds(60))
                result = $"{timeSpan.Seconds} seconds ago";

            else if (timeSpan <= TimeSpan.FromMinutes(60))
                result = timeSpan.Minutes > 1 ? $"{timeSpan.Minutes} minutes ago" : "about a minute ago";

            else if (timeSpan <= TimeSpan.FromHours(24))
                result = timeSpan.Hours > 1 ? $"{timeSpan.Hours} hours ago" : "about an hour ago";

            else
                result = date.ToString("dd-MM-yyyy HH:mm");

            return result;
        }


    }
}
