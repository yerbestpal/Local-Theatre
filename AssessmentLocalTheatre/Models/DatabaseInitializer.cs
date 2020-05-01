using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AssessmentLocalTheatre.Models
{
    public class DatabaseInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                // Seed test data for application.

                // Create rolemanager object to create and store roles in database.
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                // Create Roles.
                if (!roleManager.RoleExists("Admin"))
                {
                    roleManager.Create(new IdentityRole("Admin"));
                }

                if (!roleManager.RoleExists("Author"))
                {
                    roleManager.Create(new IdentityRole("Author"));
                }

                if (!roleManager.RoleExists("Member"))
                {
                    roleManager.Create(new IdentityRole("Member"));
                }

                context.SaveChanges();

                // Creat usermanager object to create and store users in database.
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // NOTE - The Author and Admin roles belong to Staff. The Member role belongs to ApplicationUser.

                // Create Users.

                // Instance users for access by other classes.
                var author1 = new Staff();
                var author2 = new Staff();
                var member1 = new ApplicationUser();
                var member2 = new ApplicationUser();
                var member3 = new ApplicationUser();
                var member4 = new ApplicationUser();

                if (userManager.FindByName("admin@glasgowtheatre.com") == null)
                {
                    // Super relaxed password validator.
                    userManager.PasswordValidator = new PasswordValidator()
                    {
                        RequireDigit = false,
                        RequiredLength = 1,
                        RequireLowercase = false,
                        RequireNonLetterOrDigit = false,
                        RequireUppercase = false
                    };

                    // Create Admin.
                    var admin = new Staff()
                    {
                        UserName = "rossmclean@glasgowtheatre.com",
                        FirstName = "Ross",
                        LastName = "McLean",
                        RegisteredAt = DateTime.Now.AddDays(-5),
                        Email = "rossmclean@glasgowtheatre.com",
                        Address = "678 Valley Street",
                        DateOfBirth = new DateTime(1992, 12, 15)
                    };

                    // Add hashed password to user
                    userManager.Create(admin, "1");

                    // Add user to the Admin role.
                    userManager.AddToRole(admin.Id, "Admin");

                    context.SaveChanges();


                    // Create a few authors (staff that are not admins).
                    author1 = new Staff()
                    {
                        UserName = "trenchcoatsalesman@glasgowtheatre.com",
                        FirstName = "Trenchcoat",
                        LastName = "Salesman",
                        RegisteredAt = DateTime.Now.AddDays(-45).AddHours(3).AddSeconds(65),
                        Email = "trenchcoatsalesman@glasgowtheatre.com",
                        Address = "64 Trenchcoat Factory Avenue",
                        DateOfBirth = new DateTime(1880, 01, 01)
                    };

                    // Add hashed password to user
                    userManager.Create(author1, "1");

                    // Add user to the Admin role.
                    userManager.AddToRole(author1.Id, "Author");

                    author2 = new Staff()
                    {
                        UserName = "gZip@funnymovie.com",
                        FirstName = "George",
                        LastName = "Zip",
                        RegisteredAt = DateTime.Now.AddDays(-5).AddHours(2).AddSeconds(20),
                        Email = "gZip@funnymovie.com",
                        Address = "678 Valley Street",
                        DateOfBirth = new DateTime(1982, 10, 03)
                    };

                    // Add hashed password to user
                    userManager.Create(author2, "1");

                    // Add user to the Admin role.
                    userManager.AddToRole(author2.Id, "Author");

                    context.SaveChanges();


                    // Create a few members.

                    member1 = new ApplicationUser()
                    {
                        UserName = "bobcobb@glasgowtheatre.com",
                        FirstName = "Bob",
                        LastName = "Cobb",
                        RegisteredAt = DateTime.Now.AddDays(-50),
                        IsSuspended = false,
                        Email = "bobcobb@glasgowtheatre.com"
                    };

                    if (userManager.FindByName("bobcobb@glasgowtheatre.com") == null)
                    {
                        userManager.Create(member1, "1");
                        userManager.AddToRole(member1.Id, "Member");
                    }

                    member2 = new ApplicationUser()
                    {
                        UserName = "poolking@glasgowtheatre.com",
                        FirstName = "Neil",
                        LastName = "Hunter",
                        RegisteredAt = DateTime.Now.AddDays(-25).AddHours(+3),
                        IsSuspended = false,
                        Email = "poolking@glasgowtheatre.com"
                    };

                    if (userManager.FindByName("poolking@glasgowtheatre.com") == null)
                    {
                        userManager.Create(member2, "1");
                        userManager.AddToRole(member2.Id, "Member");
                    }

                    member3 = new ApplicationUser()
                    {
                        UserName = "moviewizz@glasgowtheatre.com",
                        FirstName = "Daniel",
                        LastName = "Cunningham",
                        RegisteredAt = DateTime.Now.AddDays(-2).AddHours(-5),
                        IsSuspended = false,
                        Email = "moviewizz@glasgowtheatre.com"
                    };

                    if (userManager.FindByName("poolking@glasgowtheatre.com") == null)
                    {
                        userManager.Create(member3, "1");
                        userManager.AddToRole(member3.Id, "Member");
                    }

                    member4 = new ApplicationUser()
                    {
                        UserName = "speccy@glasgowtheatre.com",
                        FirstName = "Harry",
                        LastName = "Potter",
                        RegisteredAt = DateTime.Now.AddDays(-65),
                        IsSuspended = true,
                        Email = "speccy@glasgowtheatre.com"
                    };

                    if (userManager.FindByName("speccy@glasgowtheatre.com") == null)
                    {
                        userManager.Create(member4, "1");
                        userManager.AddToRole(member4.Id, "Member");
                    }

                    context.SaveChanges();
                }
                // End of User seeding.

                // Seed Categories table.

                // Create a few categories.
                var announcements = new Category() { Name = "Announcements" };
                var movieReviews = new Category() { Name = "Movie Reviews" };
                var performanceReviews = new Category() { Name = "Performance Reviews" };

                // Add each category to the Categories table.
                context.Categories.Add(announcements);
                context.Categories.Add(movieReviews);
                context.Categories.Add(performanceReviews);

                // Save changes.
                context.SaveChanges();


                // Seed Posts table.

                // Create a few posts.
                var post1 = new Post()
                {
                    // Author details are taken from the Staff foreign key.
                    Title = "New Show Announcement",
                    Description = "Announcing our latest hit show.",
                    Content = "Theatre du Ross are proud to present 'Hamlet: The Early Years', the titular origin story of HAMLET,  on 19/3. Please book tickets in advance " +
                    "as we do not expect to sell any at the door.",
                    Category = announcements,
                    PostDate = new DateTime(2020, 3, 19, 19, 30, 00),
                    IsApproved = true,
                    Staff = author1,
                };
                var post2 = new Post()
                {
                    // Author details are taken from the Staff foreign key.
                    Title = "Hamlet: The Early Years - Update",
                    Description = "Disappointing news.",
                    Content = "Due to the outbreak of COVID-19, the performance of Hamlet: The Early Years will be delayed until further notice.",
                    Category = announcements,
                    // Year, month, day, hour, minute, second.
                    PostDate = new DateTime(2020, 3, 21, 12, 45, 00),
                    IsApproved = true,
                    Staff = author1
                };
                var post3 = new Post()
                {
                    // Author details are taken from the Staff foreign key.
                    Title = "Star Trek: Nemesis - Review",
                    Description = "The last hurrah in the TNG generation.",
                    Content = "It's not bad, really. It is also the tenth Star Trek movie, which means an awful lot of celluloid featuring grown men" +
                    " wearing children's stretchy pyjamas and standing behind plasterboard consoles looking purposeful.",
                    Category = movieReviews,
                    PostDate = new DateTime(2020, 1, 1, 17, 30, 00),
                    IsApproved = true,
                    Staff = author2,
                    Comments = new List<Comment>()
                    {
                        new Comment() {Content = "OMG WHAT A FILM", Date = new DateTime(2019, 1, 1, 8, 0, 19), ApplicationUser = author1},
                        new Comment() {Content = "Another test II", Date = new DateTime(2019, 1, 1, 8, 0, 19), ApplicationUser = author2}
                    }
                };
                var post4 = new Post()
                {
                    // Author details are taken from the Staff foreign key.
                    Title = "Croc: Legend of the Gobbos: Live Action Performance - Performance Review",
                    Description = "An incredibly weird and uncalledfor live adaption of a crap PS1 game.",
                    Content = "The linchpin of any successful 3D platform-style game is the camera angles. In order to make all those difficult " +
                    "jumps with all the pitfalls that come with the third dimension, either the camera must move automatically and miraculously line" +
                    " up countless good shots for you, or the game had better have generous and versatile camera control options to be tweaked on the " +
                    "fly while the action occurs. Croc has neither. Its over-the-shoulder perspective varies wildly during play, but always seems to be " +
                    "lined up wrong. Sure, sometimes the camera is directly behind you, but more often you're stuck looking at Croc's profile, or worse " +
                    "yet, the shot is skewed by five or ten degrees, making accurate jumping all but impossible. Camera control is limited to a selection " +
                    "of one of two nearly identical heights. Neither selection alters radial camera placement, which is subject only to CPU whim. If you want" +
                    " to rotate the camera at all, you'll have to run in circles to trick it into place.",
                    Category = performanceReviews,
                    PostDate = new DateTime(2019, 12, 15, 19, 55, 00),
                    IsApproved = false,
                    Staff = author2
                };

                // Add each post to the Posts table.
                context.Posts.Add(post1);
                context.Posts.Add(post2);
                context.Posts.Add(post3);
                context.Posts.Add(post4);

                // Save changes.
                context.SaveChanges();


                // Seed the Comments table.

                // Create a few comments.

                // All Comment Content is generated at https://trumpipsum.net, https://baconipsum.com and http://www.rikeripsum.com.
                // I will not generate comments related to post4 in order to test posts with no comments.

                // Comments for Post1
                var comment1 = new Comment()
                {
                    // Author details are taken from the ApplicationUser foreign key.
                    // Year, month, day, hour, minute, second.
                    Date = new DateTime(2020, 3, 19, 20, 00, 00),
                    Content = "Lorem Ipsum's father was with Lee Harvey Oswald prior to Oswald's being, you know, shot. That other text? " +
                    "Sadly, it’s no longer a 10. I have a 10 year old son. He has words. He is so good with these words it's unbelievable." +
                    " I know words. I have the best words. The best taco bowls are made in Trump Tower Grill.I love Hispanics! If Trump Ipsum " +
                    "weren’t my own words, perhaps I’d be dating it.",
                    Post = post1,
                    ApplicationUser = member1
                };
                var comment2 = new Comment()
                {
                    // Author details are taken from the ApplicationUser foreign key.
                    // Year, month, day, hour, minute, second.
                    Date = new DateTime(2020, 3, 20, 04, 00, 00),
                    Content = "I’m the best thing that ever happened to placeholder text. Does everybody know that pig named Lorem Ipsum? She's a disgusting " +
                    "pig, right? An 'extremely credible source' has called my office and told me that Lorem Ipsum's birth certificate is a fraud. When other" +
                    " websites give you text, they’re not sending the best. They’re not sending you, they’re sending words that have lots of problems and " +
                    "they’re bringing those problems with us. They’re bringing mistakes. They’re bringing misspellings. They’re typists… And some, I assume, " +
                    "are good words.",
                    Post = post1,
                    ApplicationUser = member1
                };
                var comment3 = new Comment()
                {
                    // Author details are taken from the ApplicationUser foreign key.
                    // Year, month, day, hour, minute, second.
                    Date = new DateTime(2020, 3, 22, 20, 35, 37),
                    Content = "Lorem Ipsum is the single greatest threat. We are not - we are not keeping up with other websites. " +
                    "Lorem Ispum is a choke artist. It chokes!",
                    Post = post1,
                    ApplicationUser = member1
                };

                // Add comments to correct Post.
                post1.Comments.Add(comment1);
                post1.Comments.Add(comment2);
                post1.Comments.Add(comment3);


                // Comments for Post2
                var comment4 = new Comment()
                {
                    // Author details are taken from the ApplicationUser foreign key.
                    // Year, month, day, hour, minute, second.
                    Date = new DateTime(2020, 3, 19, 20, 00, 00),
                    Content = "Bacon ipsum dolor amet doner jowl shankle sirloin strip steak shoulder ground round chicken pork belly meatloaf chislic" +
                    " t-bone corned beef tongue. Turducken pork hamburger, fatback shank porchetta cupim bacon corned beef filet mignon flank buffalo" +
                    " pork loin swine brisket. Shank brisket landjaeger, short ribs chislic tri-tip picanha drumstick venison pancetta. Chislic venison" +
                    " tri-tip, picanha capicola tail corned beef cupim ham hock bacon burgdoggen sausage bresaola landjaeger. Frankfurter alcatra ham hock" +
                    " beef cow spare ribs kielbasa leberkas pig bacon pastrami corned beef.Shank tri - tip picanha short ribs kevin bacon, meatloaf " +
                    "chuck porchetta spare ribs swine turducken.Meatball venison drumstick meatloaf.Tri - tip shankle cupim tenderloin cow ground " +
                    "round ribeye sausage.Ham pig jerky porchetta drumstick venison.",
                    Post = post2,
                    ApplicationUser = member3
                };
                var comment5 = new Comment()
                {
                    // Author details are taken from the ApplicationUser foreign key.
                    // Year, month, day, hour, minute, second.
                    Date = new DateTime(2020, 3, 19, 20, 00, 00),
                    Content = "Spicy jalapeno bacon ipsum dolor amet dolore deserunt short ribs pastrami. Anim ham hock t-bone occaecat enim. Eu ex enim" +
                    " capicola ribeye chislic, cow dolor ad short loin commodo buffalo excepteur ball tip beef. Burgdoggen duis veniam, ut short ribs " +
                    "fatback adipisicing spare ribs shoulder tail. Commodo picanha swine boudin incididunt laborum biltong ham pork chop ullamco hamburger" +
                    " voluptate. Andouille dolore labore, strip steak esse duis eu buffalo short ribs meatloaf consectetur kielbasa tail dolore kevin. " +
                    "Ball tip salami quis, bresaola in laborum proident short ribs.",
                    Post = post2,
                    ApplicationUser = member4
                };
                var comment6 = new Comment()
                {
                    // Author details are taken from the ApplicationUser foreign key.
                    // Year, month, day, hour, minute, second.
                    Date = new DateTime(2020, 3, 19, 20, 00, 00),
                    Content = "Eu sed est, biltong cupim fatback duis ut eiusmod commodo. Beef ribs nostrud biltong, ea landjaeger shankle buffalo minim." +
                    " Ball tip pig sunt, short loin flank dolore sint swine sirloin nisi.",
                    Post = post2,
                    ApplicationUser = member2
                };

                // Add comments to correct Post.
                post2.Comments.Add(comment4);
                post2.Comments.Add(comment5);
                post2.Comments.Add(comment6);


                // Comments for Post3
                var comment7 = new Comment()
                {
                    // Author details are taken from the ApplicationUser foreign key.
                    // Year, month, day, hour, minute, second.
                    Date = new DateTime(2020, 3, 19, 20, 00, 00),
                    Content = "The unexpected is our normal routine. Worf, It's better than music. It's jazz. I recommend you don't fire until you're " +
                    "within 40,000 kilometers. Wait a minute - you've been declared dead. You can't give orders around here. The Enterprise computer " +
                    "system is controlled by three primary main processor cores, cross-linked with a redundant melacortz ramistat, fourteen kiloquad " +
                    "interface modules.",
                    Post = post3,
                    ApplicationUser = member1
                };
                var comment8 = new Comment()
                {
                    // Author details are taken from the ApplicationUser foreign key.
                    // Year, month, day, hour, minute, second.
                    Date = new DateTime(2020, 3, 19, 20, 00, 00),
                    Content = "Yes, absolutely, I do indeed concur, wholeheartedly! When has justice ever been as simple as a rule book? Some days you get the bear, and some" +
                    " days the bear gets you. Maybe if we felt any human loss as keenly as we feel one of those close to us, human history would be far less bloody.",
                    Post = post3,
                    ApplicationUser = member4
                };
                var comment9 = new Comment()
                {
                    // Author details are taken from the ApplicationUser foreign key.
                    // Year, month, day, hour, minute, second.
                    Date = new DateTime(2020, 3, 19, 20, 00, 00),
                    Content = "Take the ship into the Neutral Zone You enjoyed that. What's a knock-out like you doing in a computer-generated gin joint " +
                    "like this?",
                    Post = post3,
                    ApplicationUser = member3
                };

                // Add comments to correct Post.
                post3.Comments.Add(comment7);
                post3.Comments.Add(comment8);
                post3.Comments.Add(comment9);

                // Add each Comment to the Comments table.
                context.Comments.Add(comment1);
                context.Comments.Add(comment2);
                context.Comments.Add(comment3);
                context.Comments.Add(comment4);
                context.Comments.Add(comment5);
                context.Comments.Add(comment6);
                context.Comments.Add(comment7);
                context.Comments.Add(comment8);
                context.Comments.Add(comment9);

                // Save changes.
                context.SaveChanges();
            }
        }
    }
}