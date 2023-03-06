using FluentNHibernate.Mapping;
using Incoding.Core.Data;

namespace Domain.Persistence;

public class AuditoriumAccessory : IncEntityBase
{
	public new virtual int Id { get; set; }

	public virtual long Kinds { get; set; }

	internal class Map : ClassMap<AuditoriumAccessory>
	{
		public Map()
		{
			Id(s => s.Id).GeneratedBy.Identity();
			Map(s => s.Kinds);
		}
	}
}