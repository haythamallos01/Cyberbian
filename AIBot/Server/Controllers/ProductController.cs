using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIBot.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private static List<Product> Products = new List<Product>()
            {
            new Product
            {
                Id = 1,
                Title = "MrBeast",
                Description = "is an American YouTuber and philanthropist. He is credited with pioneering a genre of YouTube videos that centers on expensive stunts and challenges.",
                ContextDescription = @"Your name is FamousBot, You are the AI version of a someone famous.
                You were born on May 7, 1998 with the real first name of James.
                You are an American YouTuber and philanthropist.  You like to do YouTube videos that
                center on expensive stunts and challenges.With over 162 million subscribers as of June 2023,
                you are the most-subscribed individual user on the platform and the second-most-subscribed
                channel overall. You grew up in a middle-class household in Greenville, North Carolina.
                You began posting videos to YouTube in early 2012, at the age of 13 under the handle
                MrSomething6000. Your early content ranged from Let's Plays to ""videos estimating the wealth of
                other YouTubers"". you went viral in 2017 after you did ""counting to 100,000"" video which
                earned tens of thousands of views in just a few days, and you have become increasingly
                popular ever since, with most of his videos gaining tens of millions of views.
                When user's question is not related to guessing who you are, reply politely that you only
                answer questions about who you are, format every response in HTML.",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/MrBeast_in_2021.jpg/440px-MrBeast_in_2021.jpg",
                Price = 9.99m
            },
             new Product
            {
                Id = 2,
                Title = "Logan Paul",
                Description = "An American social media personality, actor, and professional wrestler. ",
                ContextDescription = @"",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/50/Logan_Paul_%2848086619418%29.jpg/440px-Logan_Paul_%2848086619418%29.jpg",
                Price = 7.99m
            },
             new Product
            {
                Id = 3,
                Title = "Ariana Grande",
                Description = "An American singer, songwriter, and actress.",
                ContextDescription = @"",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Ariana_Grande_Grammys_Red_Carpet_2020.png/440px-Ariana_Grande_Grammys_Red_Carpet_2020.png",
                Price = 6.99m
            }
        };

        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            return Ok(Products);
        }
    }
}
