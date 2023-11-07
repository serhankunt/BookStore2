import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ShoppingCartService } from 'src/app/services/shopping-cart.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  language:string = "en";
  
  constructor(
    private translate: TranslateService,
    public shopping:ShoppingCartService
    ) {
      if(localStorage.getItem("language")){
        // translate.setDefaultLang(localStorage.getItem("language") as string);
        console.log(localStorage);
        this.language = localStorage.getItem("language") as string;
        
      }
      else{
        localStorage.getItem("en");
      }
      translate.setDefaultLang(this.language);
  }
  
    switchLanguage(event: any) {    
    localStorage.setItem("language",event.target.value);
    console.log(event);
    this.language = event.target.value;
    this.translate.use(event.target.value);
    location.reload
  }
}
