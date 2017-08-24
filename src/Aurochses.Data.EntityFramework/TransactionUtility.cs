using System.Transactions;

namespace Aurochses.Data.EntityFramework
{
    /// <summary>
    /// Defines correct way to create transaction scope.
    /// </summary>
    public static class TransactionUtility
    {
        /// <summary>
        /// Creates transaction scope specifying proper default values.
        /// </summary>
        /// <returns></returns>
        public static TransactionScope CreateTransactionScope()
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };

            return new TransactionScope(TransactionScopeOption.Required, transactionOptions);
        }
    }
}