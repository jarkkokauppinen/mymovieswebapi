namespace mymovieswebapi

{
  public class Token
  {
    public int userID { get; set; }
    public string token { get; set; }
    public string error { get; set; }

    public Token() {}
  }
}