using System;
using System.ComponentModel.DataAnnotations;

namespace McvCore.TagHelpers.Samples.Models
{
    public class SampleViewModel
    {

    }

    public class TableSampleData
    {
        [Display(Name = "标识")]
        public int Id { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "年龄")]
        public int Age { get; set; }
    }

    public class TableQuery
    {
        [Display(Name = "标识")]
        public int Id { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "年龄")]
        public int Age { get; set; }
    }
}
