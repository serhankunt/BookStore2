export class BookModel{
    id: number = 0 ;
    title: string = "";
    author: string = "";
    summary: string = "";
    coverImageUrl : string ="";
    price : Money = new Money();
    quantity : number = 0 ;
    isActive : boolean = true;
    isDeleted : boolean = false ; 
    isbn :string = "";
    createAt : string = "";


}

export class Money{
value : number = 0 ;
currency : string = "₺";
}