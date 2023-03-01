using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Visszajelzéseket akarunk kezelni (Opinion, Bugreport, FeatureRequest)
// Mindegyik visszajelzés típushoz több fajta feldolgozó rutint akarok hozzárendelni
// Periodikusan minden tizedik (a gyakorlaton: harmadik) visszajelzés után hívjuk meg minden visszajelzésre a feldolgozó metódusokat
// A feldolgozó metódusokat egy Dictionary<Category, Action<Feedback>> adatszerkezetben akarjuk tárolni


namespace _2023_02_28_Events
{
    enum Category
    {
        OPINION,
        BUGREPORT,
        FEATUREREQ
    }

    internal class Feedback
    {
        public Feedback(Category category, string description)
        {
            this.category = category;
            this.description = description;
        }

        public Category category { get; }
        public string description { get; }
    }

    internal class FeedbackProcessor
    {
        private Dictionary<Category, Action<Feedback>> Actions;
        private List<Feedback> Feedbacks;

        public FeedbackProcessor(Action<Feedback> defaultAction)
        {
            Actions = new Dictionary<Category, Action<Feedback>>();
            foreach (Category cat in Enum.GetValues(typeof(Category)))
            {
                Actions[cat] = defaultAction;
            }
            Feedbacks = new List<Feedback>();
        }

        public void AddFeedback(Feedback fb)
        {
            Feedbacks.Add(fb);
            if (Feedbacks.Count == 3)
            {
                foreach (Feedback feedb in Feedbacks)
                {
                    Actions[feedb.category].Invoke(feedb);
                }


                Feedbacks.Clear();
            }
            
        }

        public void AddAction(Category category, Action<Feedback> action, bool doOverwrite)
        {
            if (doOverwrite) Actions[category] = action;
            else Actions[category] += action;
        }
    }

    internal class Program
    {
        static void WriteToConsole(Feedback fb)
        {
            Console.WriteLine(
                "Feedback received: " + fb.category + " - " + fb.description
            );
        }


        static void Main(string[] args)
        {
            Feedback opinion1 = new Feedback(Category.OPINION, "This software is not worth the money");
            Feedback bugreport1 = new Feedback(Category.BUGREPORT, "SW freezes when adding new user");
            Feedback featreq1 = new Feedback(Category.FEATUREREQ, "Please add a way to clear the database");
            Feedback opinion2 = new Feedback(Category.OPINION, "The best solution out there");

            FeedbackProcessor fproc = new FeedbackProcessor(fb =>
                Console.WriteLine(
                    "No Action defined for Feedbacks of category " + fb.category
                )
            ); // parameterkent adjunk at egy
               // default handlert ami kiirja hogy az adott
               // kategoriahoz meg nincs feedback

            fproc.AddAction(Category.BUGREPORT, WriteToConsole, true); // 3. parameter: doOverwrite
            fproc.AddAction(Category.FEATUREREQ, WriteToConsole, true); // ha false akkor ezt az actiont IS meghivja

            fproc.AddFeedback(opinion1);
            fproc.AddFeedback(bugreport1);
            fproc.AddFeedback(featreq1);
            fproc.AddFeedback(opinion2);

            Console.ReadLine();

        }

    }
}