 

using System;

namespace Salient.ReliableHttpClient.Serialization.Newtonsoft
{
    ///<summary>
    ///</summary>
    public static class UnixDateTimeOffsetHelper
    {
        private const string InvalidUnixEpochErrorMessage = "Unix epoc starts January 1st, 1970";
        /// <summary>
        ///   Convert a long into a DateTimeOffset
        ///   Need to double check that UTC is not required
        /// </summary>
        public static DateTimeOffset FromUnixTime(Int64 self)
        {
            var ret = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
            return ret.AddSeconds(self);
        }


        ///<summary>
        ///</summary>
        ///<param name="self"></param>
        ///<returns></returns>
        public static Int64 ToUnixTime(DateTimeOffset self)
        {
            return ToUnixTime(self,true);
        }
        /// <summary>
        ///   Convert a DateTimeOffset into a long
        ///   Need to double check that UTC is not required
        /// </summary>
        public static Int64 ToUnixTime(DateTimeOffset self,bool fixMinDate)
        {
            if (self == DateTimeOffset.MinValue)
            {
                return 0;
            }

            var epoc = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var delta = self - epoc;

            double seconds = delta.TotalSeconds;
            if (seconds < 0)
            {
                if (fixMinDate)
                {
                    seconds = 0;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(InvalidUnixEpochErrorMessage);    
                }
            }

            return (long)seconds;
        }
    }

    ///<summary>
    ///</summary>
    public static class UnixDateTimeHelper
    {
        private const string InvalidUnixEpochErrorMessage = "Unix epoc starts January 1st, 1970";
        /// <summary>
        ///   Convert a long into a DateTime
        ///   Need to double check that UTC is not required
        /// </summary>
        public static DateTime FromUnixTime( Int64 self)
        {
            var ret = new DateTime(1970, 1, 1);
            return ret.AddSeconds(self);
        }

        /// <summary>
        ///   Convert a DateTime into a long
        ///   Need to double check that UTC is not required
        /// </summary>
        public static Int64 ToUnixTime( DateTime self)
        {
            if (self == DateTime.MinValue)
            {
                return 0;
            }

            var epoc = new DateTime(1970, 1, 1);
            var delta = self - epoc;

            if (delta.TotalSeconds < 0) throw new ArgumentOutOfRangeException(InvalidUnixEpochErrorMessage);

            return (long)delta.TotalSeconds;
        }
    }
}