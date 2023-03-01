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
    internal class Program
    {
        static void Main(string[] args)
        {
            Feedback opinion1 = new Feedback(Category.OPINION, "This software is not worth the money");
            Feedback bugreport1 = new Feedback(Category.BUGREPORT, "SW freezes when adding new user");
            Feedback featreq1 = new Feedback(Category.FEATUREREQ, "Please add a way to clear the database");
            Feedback opinion2 = new Feedback(Category.OPINION, "The best solution out there");

            FeedbackProcessor fproc = new FeedbackProcessor(...); // parameterkent adjunk at egy
                                                           // default handlert ami kiirja hogy az adott
                                                           // kategoriahoz meg nincs feedback

            fproc.AddAction(Category.BugReport, WriteToConsole, true); // 3. parameter: doOverwrite
            fproc.AddAction(Category.FeatureRequest, WriteToConsole, true); // ha false akkor ezt az actiont IS meghivja

            fproc.AddFeedback(opinion1);
            fproc.AddFeedback(bugreport1);
            fproc.AddFeedback(featreq1);
            fproc.AddFeedback(opinion2);

        }

    }
}