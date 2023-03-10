using System;
using System.Collections.Generic;

namespace Nop.Core.Domain.Blogs
{
    public static class BlogExtensions
    {
        public static string[] ParseTags(this BlogPost blogPost)
        {
            if (blogPost == null)
                throw new ArgumentNullException("blogPost");

            var parsedTags = new List<string>();
            if (!String.IsNullOrEmpty(blogPost.Tags))
            {
                String tagswithoutsplit = blogPost.Tags;
                string[] tags2 = null;
                if (tagswithoutsplit.Contains(","))
                {
                    tags2 = blogPost.Tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    tags2 = blogPost.Tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                }
                
                foreach (string tag2 in tags2)
                {
                    var tmp = tag2.Trim();
                    if (!String.IsNullOrEmpty(tmp))
                        parsedTags.Add(tmp);
                }
            }
            return parsedTags.ToArray();
        }
    }
}
