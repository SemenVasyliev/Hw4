using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LectureExample.AttributeUsageExamples;

[Table("blogs", Schema = "blogging")]
public class Blog
{
    [Key]
    [Column("blog_id")]
    public int BlogId { get; set; }

    [Column(TypeName = "varchar(200)")]
    public string? Url { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal Rating { get; set; }
}
