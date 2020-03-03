using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Bulletin;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using Newtonsoft.Json.Linq;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class UpdateProtectionDocSpecialCommand : BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, KeyValuePair<string, object>[] pairs)
        {
            if (pairs is null || pairs.Length < 1)
                return;

            var repository = Uow.GetRepository<ProtectionDoc>();
            var oldProtectionDoc = await NiisAmbientContext.Current.Executor.GetCommand<GetProtectionDocByIdQuery>()
                                .Process(c => c.ExecuteAsync(protectionDocId));
            foreach (var pair in pairs)
            {
                switch (pair.Key)
                {
                    case "validDate":
                        {
                            oldProtectionDoc.ValidDate = new DateTimeOffset((DateTime)pair.Value);

                            repository.Update(oldProtectionDoc);
                            await Uow.SaveChangesAsync();
                        }
                        break;
                    case "bulletinId":
                        {
                            var relationId = oldProtectionDoc.Bulletins.FirstOrDefault(b => b.IsPublish)?.Id ?? 0;
                            if(relationId != 0)
                                await NiisAmbientContext.Current.Executor.GetCommand<DeleteProtectionDocBulletinRelationCommand>()
                                    .Process(c => c.ExecuteAsync(relationId));

                            int nextId = (int)((long)pair.Value);
                            var relation = new ProtectionDocBulletinRelation
                            {
                                BulletinId = nextId,
                                ProtectionDocId = protectionDocId,
                                IsPublish = true
                            };
                            await NiisAmbientContext.Current.Executor.GetCommand<CreateProtectionDocBulletinRelationCommand>()
                                .Process(c => c.ExecuteAsync(relation));
                        }
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
