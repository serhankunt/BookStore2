using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreServer.WebApi.Models;

public sealed class BookCategory
{
    //Composite Key
    [ForeignKey("Book")]
    public int BookId { get; set; }
    public Book Book { get; set; }
    [ForeignKey("Category")]//Buradaki foreign keyler ef te include metodunu kullanmamızı sağlıyor.(Include sql join mantığına benziyor)
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}