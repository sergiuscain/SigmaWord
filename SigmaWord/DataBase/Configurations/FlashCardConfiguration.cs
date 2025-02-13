using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SigmaWord.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaWord.DataBase.Configurations
{
    public class FlashCardConfiguration : IEntityTypeConfiguration<FlashCard>
    {
        public void Configure(EntityTypeBuilder<FlashCard> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
