import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'categoryPipe'
})
export class CategoryPipe implements PipeTransform {

  transform(value: any[], search: string) {    
    if(search === "") return value;

    return value.filter(p=> p.name.toLowerCase().includes(search.toLowerCase()));
  }
}
