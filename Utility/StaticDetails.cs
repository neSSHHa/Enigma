using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class StaticDetails
    {
        public const string AdminRole = "ADMIN";
        public const string MemberRole = "MEMBER";

        public static string Lecture_Type_Occupaed = "OCCUPAED";
        public static string Lecture_Type_Open = "OPEN";

        public static string Lecture_Status_Finished = "FINISHED";
        public static string Lecture_Status_Awaits = "AWAITS";
        public static string Lecture_status_Canceled = "CANCELED";

        public static int TimeLimit;
    }
}
