namespace Neovolve.Toolkit.Workflow.UnitTests.Activities
{
    using System;
    using System.Activities;
    using System.Activities.Statements;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="BusinessFailureEvaluatorTests"/>
    ///   class is used to test the <see cref="BusinessFailureEvaluator{T}"/> class.
    /// </summary>
    [TestClass]
    public class BusinessFailureEvaluatorTests
    {
        /// <summary>
        /// Runs test for business failure evaluator completes when condition is false.
        /// </summary>
        [TestMethod]
        public void BusinessFailureEvaluatorCompletesWhenConditionIsFalseTest()
        {
            BusinessFailureEvaluatorWithFailure target = new BusinessFailureEvaluatorWithFailure();
            Dictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("Condition", false);
            inputParameters.Add("Failure", CreateDefaultFailure());

            WorkflowInvoker.Invoke(target, inputParameters);
        }

        /// <summary>
        /// Runs test for business failure evaluator must define failure code or description binding.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidWorkflowException))]
        public void BusinessFailureEvaluatorMustDefineFailureCodeOrDescriptionBindingTest()
        {
            BusinessFailureEvaluator<Int32> target = new BusinessFailureEvaluator<Int32>();

            ActivityInvoker.Invoke(target);
        }

        /// <summary>
        /// Runs test for business failure evaluator provides failure to extension when hosted in business failure scope.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(BusinessFailureException<Int32>))]
        public void BusinessFailureEvaluatorProvidesFailureToExtensionWhenHostedInBusinessFailureScopeTest()
        {
            BusinessFailureScope<Int32> target = new BusinessFailureScope<Int32>
                                                 {
                                                     Activities =
                                                         {
                                                             new BusinessFailureEvaluator<Int32>
                                                             {
                                                                 Code = new InArgument<Int32>(123), 
                                                                 Description = new InArgument<String>("Description")
                                                             }
                                                         }
                                                 };

            WorkflowInvoker.Invoke(target);
        }

        /// <summary>
        /// Runs test for business failure evaluator throws exception directly if extension exists but is not within scope.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(BusinessFailureException<Int32>))]
        public void BusinessFailureEvaluatorThrowsExceptionDirectlyIfExtensionExistsButIsNotWithinScopeTest()
        {
            Sequence target = new Sequence
                              {
                                  Activities =
                                      {
                                          new BusinessFailureScope<Int32>(), 
                                          new BusinessFailureEvaluator<Int32>
                                          {
                                              Code = new InArgument<Int32>(123123), 
                                              Description = new InArgument<String>("Description value")
                                          }
                                      }
                              };

            WorkflowInvoker.Invoke(target);
        }

        /// <summary>
        /// Runs test for business failure evaluator throws exception when code is bound and description is not.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidWorkflowException))]
        public void BusinessFailureEvaluatorThrowsExceptionWhenCodeIsBoundAndDescriptionIsNotTest()
        {
            BusinessFailureEvaluator<Int32> target = new BusinessFailureEvaluator<Int32>
                                                     {
                                                         Code = new InArgument<Int32>(234234)
                                                     };

            ActivityInvoker.Invoke(target);
        }

        /// <summary>
        /// Runs test for business failure evaluator throws exception when condition is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(BusinessFailureException<Int32>))]
        public void BusinessFailureEvaluatorThrowsExceptionWhenConditionIsNullTest()
        {
            BusinessFailureEvaluatorWithFailure target = new BusinessFailureEvaluatorWithFailure();
            Dictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("Failure", CreateDefaultFailure());

            WorkflowInvoker.Invoke(target, inputParameters);
        }

        /// <summary>
        /// Runs test for business failure evaluator throws exception when condition is true.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(BusinessFailureException<Int32>))]
        public void BusinessFailureEvaluatorThrowsExceptionWhenConditionIsTrueTest()
        {
            BusinessFailureEvaluatorWithFailure target = new BusinessFailureEvaluatorWithFailure();
            Dictionary<String, Object> inputParameters = new Dictionary<String, Object>();

            inputParameters.Add("Condition", true);
            inputParameters.Add("Failure", CreateDefaultFailure());

            WorkflowInvoker.Invoke(target, inputParameters);
        }

        /// <summary>
        /// Runs test for business failure evaluator throws exception when description is bound but code is not.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidWorkflowException))]
        public void BusinessFailureEvaluatorThrowsExceptionWhenDescriptionIsBoundButCodeIsNotTest()
        {
            BusinessFailureEvaluator<Int32> target = new BusinessFailureEvaluator<Int32>
                                                     {
                                                         Description = new InArgument<String>("Description value")
                                                     };

            ActivityInvoker.Invoke(target);
        }

        /// <summary>
        /// Runs test for business failure evaluator throws exception when failure and code is bound.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidWorkflowException))]
        public void BusinessFailureEvaluatorThrowsExceptionWhenFailureAndCodeIsBoundTest()
        {
            BusinessFailureEvaluator<Int32> target = new BusinessFailureEvaluator<Int32>
                                                     {
                                                         Code = new InArgument<Int32>(234234), 
                                                         Failure = new InArgument<BusinessFailure<Int32>>(CreateDefaultFailure())
                                                     };

            ActivityInvoker.Invoke(target);
        }

        /// <summary>
        /// Runs test for business failure evaluator throws exception when failure and description is bound.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidWorkflowException))]
        public void BusinessFailureEvaluatorThrowsExceptionWhenFailureAndDescriptionIsBoundTest()
        {
            BusinessFailureEvaluator<Int32> target = new BusinessFailureEvaluator<Int32>
                                                     {
                                                         Description = new InArgument<String>("Description value"), 
                                                         Failure = new InArgument<BusinessFailure<Int32>>(CreateDefaultFailure())
                                                     };

            ActivityInvoker.Invoke(target);
        }

        /// <summary>
        /// Runs test for business failure evaluator throws exception when hosted in scope of different type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ActivityFailureException))]
        public void BusinessFailureEvaluatorThrowsExceptionWhenHostedInScopeOfDifferentTypeTest()
        {
            Guid guidCode = Guid.NewGuid();

            BusinessFailureScope<Guid> target = new BusinessFailureScope<Guid>
                                                {
                                                    Activities =
                                                        {
                                                            new BusinessFailureEvaluator<Guid>
                                                            {
                                                                Code = new InArgument<Guid>(guidCode), 
                                                                Description = new InArgument<String>("Description")
                                                            }, 
                                                            new BusinessFailureEvaluator<Int32>
                                                            {
                                                                Code = new InArgument<Int32>(234234), 
                                                                Description = new InArgument<String>("Test value")
                                                            }
                                                        }
                                                };

            try
            {
                ActivityInvoker.Invoke(target);
            }
            catch (ActivityFailureException ex)
            {
                BusinessFailureException<Int32> failureException = ex.InnerException as BusinessFailureException<Int32>;

                Assert.IsNotNull(failureException, "Incorrect exception type thrown");

                throw;
            }
        }

        /// <summary>
        /// Runs test for business failure evaluator throws exception with provided failure details.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(BusinessFailureException<Int32>))]
        public void BusinessFailureEvaluatorThrowsExceptionWithProvidedFailureDetailsTest()
        {
            BusinessFailureEvaluatorWithDescription target = new BusinessFailureEvaluatorWithDescription();
            Dictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            BusinessFailure<Int32> failure = CreateDefaultFailure();

            inputParameters.Add("Code", failure.Code);
            inputParameters.Add("Description", failure.Description);

            try
            {
                WorkflowInvoker.Invoke(target, inputParameters);
            }
            catch (BusinessFailureException<Int32> ex)
            {
                Assert.AreEqual(failure.Code, ex.Failures.First().Code, "Code returned an incorrect value");
                Assert.AreEqual(failure.Description, ex.Failures.First().Description, "Description returned an incorrect value");

                throw;
            }
        }

        /// <summary>
        /// Runs test for business failure evaluator throws exception with provided failure.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(BusinessFailureException<Int32>))]
        public void BusinessFailureEvaluatorThrowsExceptionWithProvidedFailureTest()
        {
            BusinessFailureEvaluatorWithFailure target = new BusinessFailureEvaluatorWithFailure();
            Dictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            BusinessFailure<Int32> failure = CreateDefaultFailure();

            inputParameters.Add("Failure", failure);

            try
            {
                WorkflowInvoker.Invoke(target, inputParameters);
            }
            catch (BusinessFailureException<Int32> ex)
            {
                Assert.AreSame(failure, ex.Failures.First(), "Exception does not contain expected failure");

                throw;
            }
        }

        #region Static Helper Methods

        /// <summary>
        /// Creates the default failure.
        /// </summary>
        /// <returns>
        /// A <see cref="BusinessFailure&lt;T&gt;"/> instance.
        /// </returns>
        private static BusinessFailure<Int32> CreateDefaultFailure()
        {
            return new BusinessFailure<Int32>(0, Guid.NewGuid().ToString());
        }

        #endregion

        /// <summary>
        ///   Gets or sets the test context which provides
        ///   information about and functionality for the current test run.
        /// </summary>
        /// <value>
        ///   The test context.
        /// </value>
        public TestContext TestContext
        {
            get;
            set;
        }
    }
}