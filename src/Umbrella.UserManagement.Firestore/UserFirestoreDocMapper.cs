using Umbrella.UserManagement;
using Umbrella.Infrastructure.Firestore.Abstractions;
using Umbrella.Infrastructure.Firestore.Extensions;

namespace Umbrella.UserManagement.Firestore
{
    /// <summary>
    /// Mapper between DTO and Document for entity User
    /// </summary>
    internal class UserFirestoreDocMapper : IFirestoreDocMapper<UserDto, UserFirestoreDocument>
    {
        /// <summary>
        /// Gets the DTO
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public UserDto FromFirestoreDoc(UserFirestoreDocument doc)
        {
            if(doc is null)
                return null;

            var dto = new UserDto()
            {     
                Name = doc.Name,
                DisplayName = doc.DisplayName,
                LastLoginDate = doc.LastLoginDate,
                ImageUrl = doc.ImageUrl,
                CreationDate = doc.CreatedOn,
                LastUpdateDate = doc.LastUpdatedOn
            };
            dto.Roles.Clear();
            dto.Roles.AddRange(doc.Roles);
            return dto;
        }
        /// <summary>
        /// Gets the Document
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public UserFirestoreDocument ToFirestoreDocument(UserDto dto)
        {
            if(dto is null)
                return null;
                
            var doc = new UserFirestoreDocument()
            {
                Name = dto.Name,
                DisplayName = dto.DisplayName,
                ImageUrl = dto.ImageUrl,  
            };
            doc.LastLoginDate = dto.LastLoginDate.ToFirestoreTimeStamp();
            doc.CreatedOn = dto.CreationDate.ToFirestoreTimeStamp();
            doc.LastUpdatedOn = dto.LastUpdateDate.HasValue ? dto.LastUpdateDate.Value.ToFirestoreTimeStamp() : null;
            doc.SetDocumentId(dto.Name.ToString());
            doc.Roles.Clear();
            doc.Roles.AddRange(dto.Roles);
            return doc;
        }
    }
}