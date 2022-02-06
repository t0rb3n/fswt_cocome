import {HttpClient} from "@angular/common/http";
import {Inject, Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root',
})
export class GlobalConfig {
  server: StoreConfig = {storeName: 'none'};

  constructor(private http: HttpClient, @Inject('BASE_API_URL') private baseUrl: string) {
    this.http.get<StoreConfig>(this.baseUrl + 'config').subscribe(
      result => {
        this.server = result;
      }
    )
  }
}

export interface StoreConfig {
  storeName: string,
  storeLocation?: string,
  enterpriseName?: string
}

export class Title {
  static set(text: string){
    document.getElementsByTagName('title')[0].innerText = `${text} | StoreApp`;
  }
}
