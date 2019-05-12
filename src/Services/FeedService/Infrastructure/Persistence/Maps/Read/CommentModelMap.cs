using FeedService.Domain.Read.Models;
using FluentNHibernate.Mapping;

namespace FeedService.Infrastructure.Persistence.Maps.Read
{
    public class CommentModelMap: ClassMap<CommentModel>
    {
        public CommentModelMap()
        {
            ReadOnly();
            Table("Comment");
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Text);
            Map(x => x.PersonId);
            References(x => x.CommentReply, "PostId");
        }
    }
}