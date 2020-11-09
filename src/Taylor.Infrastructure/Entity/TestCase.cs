using System;
using System.Collections.Generic;
using System.Text;
using Taylor.Core.DapperExtensions;

namespace Taylor.Infrastructure.Entity
{
    [Table("dev_test_case")]
    public class TestCase : IEntity<int>
    {
        [Key, Column("idx")]
        public int Id { get; set; }

        [ Column("tc_id")]
        public string CaseId { get; set; }

        [Column("tc_title")]
        public string Title { get; set; }

        [Column("tc_description")]
        public string Description { get; set; }

        [Column("req_id")]
        public string RequirementId { get; set; }

        [Column("created_by")]
        public string CreatedBy { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_by")]
        public string UpdatedBy { get; set; }


        [Column("updated_date")]
        public DateTime UpdatedDate { get; set; }
    }
}
