using FeedService.Domain.Write.States;
using FluentNHibernate.Mapping;

namespace FeedService.Infrastructure.Persistence.Maps.Write
{
    public class PostStateMap: ClassMap<PostState>
    {
        public PostStateMap()
        {
            Table("Post");
            Id(x => x.PostId).GeneratedBy.Assigned();
            Map(x => x.Text);
            Map(x => x.PersonId);

            HasMany(x => x.Comments)
                .Inverse()
                .KeyColumns.Add("PostId", x => x.Not.Nullable())
                .Cascade.AllDeleteOrphan();
        }
    }
}