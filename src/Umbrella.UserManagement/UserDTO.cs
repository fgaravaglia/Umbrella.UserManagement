using System.Security.Claims;

namespace Umbrella.Security
{
    public class UserDTO
    {
        public string Name {get; set;}

        public string DisplayName {get; set;}

        public string ImageUrl {get; set;}

        public DateTime LastLoginDate {get; set;}

        public List<string> Roles {get; set;}

        public DateTime CreationDate {get; set;}

        public DateTime? LastUpdateDate {get; set;}

        public UserDTO()
        {
            this.DisplayName = "";
            this.Name = "";
            this.LastLoginDate = DateTime.MinValue;
            this.ImageUrl = "~/img/user1-128x128.jpg";
            this.Roles = new  List<string>(){"USER"};
        }

        public static UserDTO FromClaimsIdentity(ClaimsPrincipal identity)
        {
            if(identity is null)
                throw new ArgumentNullException(nameof(identity));
            
            var user = new UserDTO();
            user.Name = identity.Identity.Name;

            var nameClaim = identity.Claims.SingleOrDefault(x => x.Type == "name");
            user.DisplayName = nameClaim is null ? user.Name : nameClaim.Value;

            var imageClaim = identity.Claims.SingleOrDefault(x => x.Type == "picture");
            user.SetImageUrl(imageClaim is null ? "" : imageClaim.Value);
            return user;
        }
        
        public void SetImageUrl(string url)
        {
            if(String.IsNullOrEmpty(url))
                this.ImageUrl = "~/img/user1-128x128.jpg";
            else
                this.ImageUrl = url;
        }
    }
}