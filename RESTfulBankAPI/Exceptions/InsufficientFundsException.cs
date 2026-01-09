namespace RESTfulBankAPI.Exceptions;

public class InsufficientFundsException(string message, string paramName) : ArgumentException(message, paramName);