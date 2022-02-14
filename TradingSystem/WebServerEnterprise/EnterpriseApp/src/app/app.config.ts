import {HttpClient} from "@angular/common/http";
import {Inject, Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root',
})
export class GlobalConfig {
  server: EnterpriseConfig = {enterpriseName: 'none'};

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.http.get<EnterpriseConfig>(this.baseUrl + 'config').subscribe(
      result => {
        this.server = result;
      }
    )
  }
}

export interface EnterpriseConfig {
  enterpriseName: string
}

export class Title {
  static set(text: string){
    document.getElementsByTagName('title')[0].innerText = `${text} | EnterpriseApp`;
  }
}
