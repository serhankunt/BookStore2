import { BookModel } from "./book.model"

export class PaymentModel{
    books:BookModel[]=[];
    buyer : BuyerModel = new BuyerModel();
    shippingAddress : AddressModel = new AddressModel();
    billingAddress: AddressModel = new AddressModel();
    paymentCard : PaymentCardModel = new PaymentCardModel();
}

export class BuyerModel{
id:string = "";
name:string = "Hüseyin Serhan";
surname:string = "Kunt";
identityNumber:string = "11111111111";
email:string = "hserhan006@gmail.com";
gsmNumber:string = "5422504404";
registrationDate:string = "";
lastLoginDate:string = "";
registrationAddress:string = "";
city:string = "";
country:string = "";
zipCode:string = "";
ip:string = "";
}
export class AddressModel {
    description: string = "Ankara";
    zipCode: string = "06730";
    contactName: string = "H.Serhan Kunt";
    city: string = "Ankara";
    country: string = "Türkiye";
}

export class PaymentCardModel {
    cardHolderName: string = "Hüseyin Serhan Kunt";
    cardNumber: string = "";
    expireYear: string = "";
    expireMonth: string = "";
    cvc: string = "377";
}