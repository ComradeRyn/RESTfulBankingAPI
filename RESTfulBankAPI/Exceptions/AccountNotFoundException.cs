namespace RESTfulBankAPI.Exceptions;

public class AccountNotFoundException(string message, string paramName) : ArgumentException(message, paramName);