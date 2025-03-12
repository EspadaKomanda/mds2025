using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PurpleHackBackend.Models.Database;
using PurpleHackBackend.Services.User;

namespace PurpleHackBackend.Database.Extensions;

public static class ChangeTrackerExtensions
{
    public static void SetAuditProperties(this ChangeTracker changeTracker, ICurrentUserService currentUserService)
    {
        changeTracker.DetectChanges();
        IEnumerable<EntityEntry> entities =
            changeTracker
                .Entries()
                .Where(t => t.Entity is AuditableEntity &&
                            (
                                t.State == EntityState.Deleted
                                || t.State == EntityState.Added
                                || t.State == EntityState.Modified
                            ));

        if (entities.Any())
        {
            DateTimeOffset timestamp = DateTimeOffset.UtcNow;

            string user = currentUserService.GetCurrentUser().Login ?? "Unknown";

            foreach (EntityEntry entry in entities)
            {
                AuditableEntity entity = (AuditableEntity)entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedOn = timestamp;
                        entity.CreatedBy = user;
                        entity.UpdatedOn = timestamp;
                        entity.UpdatedBy = user;
                        break;
                    case EntityState.Modified:
                        entity.UpdatedOn = timestamp;
                        entity.UpdatedBy = user;
                        break;
                    case EntityState.Deleted:
                        entity.UpdatedOn = timestamp;
                        entity.UpdatedBy = user;
                        entry.State = EntityState.Deleted;
                        break;
                }
            }
        }
    }
}
