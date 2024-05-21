using Ems.Domain.Shared;

namespace Ems.Domain.Errors;

public static class DomainErrors
{

    public static class TransactionDate
    {
        public static readonly Error Empty = new(
            "TransactionDate.Empty",
            "TransactionDate is empty.");

        public static readonly Error InvalidFormat = new(
            "TransactionDate.InvalidFormat",
            "Transaction Date format is invalid.");
    }

    public static class TransactionNumber
    {
        public static readonly Error Empty = new(
            "TransactionNumber.Empty",
            "Transaction Number is empty.");

        public static readonly Error TooLong = new(
            "TransactionNumber.TooLong",
            "Transaction Number is too long.");
    }
    public static class TransactionAmount
    {
        public static readonly Error Empty = new(
            "TransactionAmount.Empty",
            "Transaction Amount is empty.");

        public static readonly Error TooLong = new(
            "TransactionAmount.TooLong",
            "Transaction Amount is too long.");
    }
    public static class Balance
    {
        public static readonly Error Empty = new(
            "Balance.Empty",
            "Balance Amount is empty.");

        public static readonly Error TooLong = new(
            "Balance.TooLong",
            "Balance Amount is too long.");
    }
    public static class TransactionDetails
    {
        public static readonly Error Empty = new(
            "TransactionNumber.Empty",
            "Transaction Number is empty.");

    }

}