using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogDbContext blogDbContext;

        public BlogPostRepository(BlogDbContext _blogDbContext)
        {
            blogDbContext = _blogDbContext;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await blogDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await blogDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await blogDbContext.AddAsync(blogPost);
            await blogDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await blogDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandel = blogPost.UrlHandel;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishDate = blogPost.PublishDate;
                existingBlog.Tags = blogPost.Tags;

                await blogDbContext.SaveChangesAsync();
                return existingBlog;
            }

            return null;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlog = await blogDbContext.BlogPosts.FindAsync(id);

            if (existingBlog!=null)
            {
                blogDbContext.BlogPosts.Remove(existingBlog);
                blogDbContext.SaveChangesAsync();

                return existingBlog;
            }
            return null;
        }
    }
}
