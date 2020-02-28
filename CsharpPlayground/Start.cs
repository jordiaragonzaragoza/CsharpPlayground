
using System.Threading.Tasks;
using RaisingEventsWithExceptionHandlingSubscriber;

namespace CsharpPlayground
{
    using CustomEventArguments;
    using Generics;
    using ExtensionMethods;
    using OverridingMethods;
    using ImplicitExplicitConversion;
    using InterfaceExplicitly;
    using IntefaceAndGenerics;
    using ImplementingCollectionInterfaces;
    using LearningDelegates;
    using LambdaIntroduction;
    using LearningAttributes;
    using ThrowingCustomExceptionWithTheOriginal;
    using LearningReflection;
    using DisposeAndFinalize;
    using StringsAdvanced;
    using LearningEvents;
    using LearningAsyncAwait;

    public class Start
    {
       
        public static async Task Main(string[] args)
        {
            //LearningDelegates.Start();

            //LearningEvents.Start();

            //UsingDelegate.Start();

            //MulticastDelegate.Start();

            //ActionExposeEventSubscriber.Start();

            //CustomEventArguments.Start();

            //RaisingEventsWithExceptionHandling.Start();

            //LambdaIntroduction.Start();

            //SubscriberRaisingEventsWithExceptionHandling.Start();

            //ExceptionHandlingInspecting.Start();

            //ExceptionDispatchInfoThrow.Start();

            //ThrowingCustomExceptionWithTheOriginal.Start();

            //Generics.Start();

            //ExtensionMethods.Start();

            //OverridingMethods.Start();

            //ImplicitExplicitConversion.Start();

            //InterfaceExplicitly.Start();

            //IntefaceAndGenerics.Start();

            //ImplementingCollectionInterfaces.Start();

            //LearningAttributes.Start();

            //LearningReflection.Start();

            //DisposeAndFinalize.Start();

            //StringsAdvanced.Start();

            await LearningAsyncAwait.Start();
        }
    }
}
