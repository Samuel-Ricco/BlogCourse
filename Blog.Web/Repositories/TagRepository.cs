using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext blogDbContext;

        public TagRepository(BlogDbContext _blogDbContext)
        {
            blogDbContext = _blogDbContext;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await blogDbContext.Tags.ToListAsync();
        }

        public Task<Tag> GetAsync(Guid id)
        {
            return blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag> addAsync(Tag tag)
        {
            await blogDbContext.Tags.AddAsync(tag);
            await blogDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await blogDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await blogDbContext.SaveChangesAsync();

                return existingTag;
            }

            return null;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = await blogDbContext.Tags.FindAsync(id);
            if (existingTag != null)
            {
                blogDbContext.Tags.Remove(existingTag);
                await blogDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }
    }
}
