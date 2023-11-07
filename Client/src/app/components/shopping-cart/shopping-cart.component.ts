import { Component } from '@angular/core';
import { ShoppingCartService } from '../../services/shopping-cart.service';
import { TranslateService } from '@ngx-translate/core';
import { PaymentModel } from 'src/app/models/payment.model';
import { Cities, Countries } from 'src/app/constants/address';
import { SwalService } from 'src/app/services/swal.service';



@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent {
  [key:string] : any;
  language: string = "en";
  selectedTab : number  = 1;
  request:PaymentModel = new PaymentModel();
  countries = Countries;
  cities = Cities ;
  isSameAddress : boolean = false;
  cartNumber1:string = "5890";
  cartNumber2:string = "0400";
  cartNumber3:string = "0000";
  cartNumber4:string = "0016";
  expireMonthAndYear:string = "2025-07";
  selectedCurrencyForPayment :string = "₺";


  constructor(
    public shopping: ShoppingCartService,
    private translate: TranslateService,
    private swal : SwalService
  ){ 
    if(localStorage.getItem("language")){
      this.language = localStorage.getItem("language") as string;
    }    

    this.shopping.checkLocalStoreForShoppingCarts();
    this.request.books = this.shopping.shoppingCarts;
  }
  changeTab(tabNumber:number | any){
    this.selectedTab = tabNumber;
  }

  setSelectedPaymentCurrency(currency:string){
    this.selectedCurrencyForPayment  = currency;
    const newBooks = this.shopping.shoppingCarts.filter(p=>p.price.currency === this.selectedCurrencyForPayment);
    this.request.books = newBooks;
  }

  payment(){
    this.request.paymentCard.expireMonth = this.expireMonthAndYear.substring(5);
    this.request.paymentCard.expireYear = this.expireMonthAndYear.substring(0,4);
    this.request.paymentCard.cardNumber = this.cartNumber1 + this.cartNumber2 + this.cartNumber3 + this.cartNumber4;
    this.request.buyer.registrationAddress = this.request.shippingAddress.description ;
    this.request.buyer.city = this.request.shippingAddress.city;
    this.request.buyer.country = this.request.shippingAddress.country;
    
    this.shopping.payment(this.request,(res)=>{
      const btn = document.getElementById("paymentModalCloseBtn");
      btn?.click();
      const remainShoppingCarts = this.shopping.shoppingCarts.filter(p=>p.price.currency !== this.selectedCurrencyForPayment);
      localStorage.setItem("shoppingCarts",JSON.stringify(remainShoppingCarts));
      this.shopping.checkLocalStoreForShoppingCarts();
      this.translate.get("paymentIsSuccessful").subscribe(translate =>{
        this.swal.callToast(translate,"success")
      });
      
    })
  }
  
  changeIsSameAddress(){
    if(this.isSameAddress){
      this.request.billingAddress = {...this.request.shippingAddress};
      
    }
  }
  
  gotoNextInputIf4Length(inputCount:string="",value : string = ""){
    this[`cartNumber${inputCount}`] = value.replace(/[^0-9]/g.toString(),"");
    value = value.replace(/[^0-9]/g,"");

    if(value.length === 4){
      if(inputCount ==="4"){
        const el = document.getElementById("expireMonthAndYear");
        el?.focus();
      }else{
        const id:string = `cartNumber${+inputCount + 1}`;
        const el: HTMLInputElement = document.getElementById(id) as HTMLInputElement;
        el.focus();
      }
    }
  }
  setExpireMonthAndYear(){
  
      this.expireMonthAndYear  = this.expireMonthAndYear.replace(/[^0-9]/g,"");
     
     if(this.expireMonthAndYear.length>2){
      this.expireMonthAndYear = this.expireMonthAndYear.substring(0,2) + "/" + this.expireMonthAndYear.substring(2);
     }
     if(this.expireMonthAndYear.length>=2){
      const month = parseInt(this.expireMonthAndYear.substring(0,2));
      if(month===0){
        this.expireMonthAndYear = "01"+this.expireMonthAndYear.substring(2);
      }
      else if(month>12){
        this.expireMonthAndYear = "12"+this.expireMonthAndYear.substring(2);
      }
     }
     if(this.expireMonthAndYear.length>4){
      const el = document.getElementById("cvc");
      el?.focus();
     }
   
  }
}