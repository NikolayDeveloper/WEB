using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings
{
    public class SystemSettingsMap:IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            //Ограничение поля тип константы по уникальности
            modelBuilder.Entity<SystemSettings>()
                .HasIndex(x => x.SettingType)
                .IsUnique();
        }
    }
}
