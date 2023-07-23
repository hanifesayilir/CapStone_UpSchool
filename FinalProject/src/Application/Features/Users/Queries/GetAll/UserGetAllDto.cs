namespace Application.Features.Users.Queries.GetAll
{
    public class UserGetAllDto
    {

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public UserGetAllDto(string id,string firstName, string lastName, string email, DateTimeOffset createdOn)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            CreatedOn = createdOn;
        }

        public UserGetAllDto()
        {
        }
    }
}
