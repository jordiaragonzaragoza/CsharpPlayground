namespace CsharpPlayground
{
    using System;
    using System.Threading.Tasks;
    using RaisingEventsWithExceptionHandling;
    using CustomEventArguments;
    using InheritanceEvents;
    using Generics;
    using ExtensionMethods;
    using OverridingMethods;
    using ImplicitExplicitConversion;
    using InterfaceExplicitly;
    using IntefaceAndGenerics;
    using ImplementingCollectionInterfaces;
    using DelegateActionPredicateFunc;
    using LambdaSimpleFunc;
    using Attributes;
    using ThrowingCustomExceptionWithTheOriginal;
    using Reflection;
    using DisposeAndFinalize;
    using StringsAdvanced;
    using DelegateEvent;
    using SimpleAsyncAwait;
    using Threading;
    using LearningTask;
    using CovarianceContravariance;
    using DelegateSimple;
    using ActionExposeEvent;
    using ExceptionHandlingInspecting;
    using ExceptionDispatchInfoThrow;
    using HandleProperties;
    using WeakReferences;
    using TaskInstantiation;

    public class Start
    {
        public static async Task Main(string[] args)
        {
            ////** Delegates And Events **////

            //DelegateSimple.Start();

            //MulticastDelegate.Start();

            //DelegateActionPredicateFunc.Start();

            //DelegateEvent.Start();

            //CustomEventArguments.Start();

            //InheritanceEvents.Start();

            //RaisingEventsWithExceptionHandling.Start();

            //ActionExposeEventSubscriber.Start();

            //LambdaSimpleFunc.Start();

            ////** Exceptions **////

            //ExceptionHandlingInspecting.Start();

            //ExceptionDispatchInfoThrow.Start();

            //ThrowingCustomExceptionWithTheOriginal.Start();

            ////** CreateTypes **////

            //ExtensionMethods.Start();

            //OverridingMethods.Start();

            //Generics.Start();

            ////** Class Hierarchy **////

            //ImplicitExplicitConversion.Start();

            //InterfaceExplicitly.Start();

            //IntefaceAndGenerics.Start();

            //ImplementingCollectionInterfaces.Start();

            //HandleProperties.Start();

            ////** Attributes and Reflection **////

            //Attributes.Start();

            //Reflection.Start();

            ////** Garbage Collector **////

            //DisposeAndFinalize.Start();

            //WeakReferences.Start();

            ////** Manipulate Strings **////

            //StringsAdvanced.Start();

            ////** Threads And Tasks **////

            await SimpleAsyncAwait.Start();

            //await TaskInstantiation.Start();

            //Threading.Start();

            //LearningTask.Start();

            //CovarianceContravariance.Start();
        }
    }
}
