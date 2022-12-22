using Google.Cloud.Firestore;
using Umbrella.Infrastructure.Firestore;
using Umbrella.Infrastructure.Firestore.Abstractions;

namespace Umbrella.UserManagement.Firestore
{
    /// <summary>
    /// Document that is persisted on Firestore as a valid user
    /// </summary>
    [FirestoreData]
    public class UserFirestoreDocument : FirestoreDocument
    {
        [FirestoreProperty]
        public string DisplayName { get; set; }

        [FirestoreProperty]
        public string ImageUrl { get; set; }

        [FirestoreProperty]
        public DateTime LastLoginDate { get; set; }

        [FirestoreProperty]
        public List<string> Roles { get; set; }

        /// <summary>
        /// Default COnstr
        /// </summary>
        public UserFirestoreDocument() : base()
        {
            this.DisplayName = "";
            this.LastLoginDate = DateTime.MinValue;
            this.ImageUrl = "";
            this.Roles = new List<string>() { "USER" };
        }
        /// <summary>
        /// Sets the ID
        /// </summary>
        /// <param name="id"></param>
        public override void SetDocumentId(string id)
        {
            base.SetDocumentId(id);
            this.Name = id;
        }
    }
}