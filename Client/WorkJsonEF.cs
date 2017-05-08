using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{

    public class WorkJsonEF
    {
        public int status { get; set; }
        public string message { get; set; }
        public WorkJsonEF_Datum[] data { get; set; }
    }

    public class WorkJsonEF_Datum
    {
        public string CourseName { get; set; }
        public WorkJsonEF_Datum_Datum1[] Data { get; set; }
        public object url { get; set; }
        public int Count { get; set; }
    }

    public class WorkJsonEF_Datum_Datum1
    {
        public int CourseExerciseID { get; set; }
        public int? CourseID { get; set; }
        public string ExerciseID { get; set; }
        public int? MaxTimesOfTrying { get; set; }
        public int PaperLimitingTime { get; set; }
        public int? ExerciseType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ExerciseName { get; set; }
        public int? PaperBuildingMode { get; set; }
        public int SubmitCount { get; set; }
        public int MaxScore { get; set; }
        public int IsCanDoHomework { get; set; }
        public string CourseWareLocation { get; set; }
        public string ceshu { get; set; }
    }

}
