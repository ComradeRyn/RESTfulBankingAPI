namespace RESTfulBankAPI.Exceptions;

public class NegativeAmountException(string message, string paramName) : ArgumentException(message, paramName);