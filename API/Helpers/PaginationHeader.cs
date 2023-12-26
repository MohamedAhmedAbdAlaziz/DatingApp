namespace API.Helpers
{
    public class PaginationHeader
    {
     public PaginationHeader(int curentPage, int itemsPerPage, int totalItems, int totalPages) 
        {
         CurentPage = curentPage;
         ItemsPerPage = itemsPerPage;
         TotalItems = totalItems;
         TotalPages = totalPages;
               
        }
        public int CurentPage {get;set;}
        public int ItemsPerPage {get;set;}
        public int TotalItems {get;set;}
        public int TotalPages {get;set;}
    }
}