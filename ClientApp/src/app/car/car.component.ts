import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Observable } from 'rxjs';
import { AuthorizeService } from '../../api-authorization/authorize.service';

@Component({
  selector: 'app-car-component',
  templateUrl: './car.component.html',
  styleUrls: ['./car.component.scss']
})
export class CarComponent {
  public car: Car;
  id: string;
  carAvailable: boolean;
  today: Date;
  dates = { from: undefined, to: undefined };
  public isAuthenticated: Observable<boolean>;

  constructor(private authorizeService: AuthorizeService, private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private _snackBar: MatSnackBar) {
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.carAvailable = false;
    this.today = new Date();

  }
  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.http.get<Car>(this.baseUrl + `api/car/${this.id}`).subscribe(result => {
      this.car = result;
    }, error => console.error(error));
  }

  createImagePath(path: string) {
    path.replace("\\", "/");
    return `${this.baseUrl}${path}`;
  }

  reserveCar() {
    this.http.post<Rent>(this.baseUrl + 'api/rent', { carID: this.id, DateFrom: this.dates.from, DateTo: this.dates.to }).subscribe(() => {
      this.openSnackBar("The car has been reserved for you", "ok");
    }, error => { this.openSnackBar(error.error.title, "ok"); });
  }

  checkIfAvailable() {
    if (this.dates.from != undefined && this.dates.to != undefined) {
      const params = new HttpParams({ fromObject: { dateFrom: this.dates.from, dateTo: this.dates.to } });
      this.http.get<boolean>(this.baseUrl + `api/rent/${this.id}/availability`, { params }).subscribe((result: boolean) => {
        this.carAvailable = result;
        if (!result) this.openSnackBar("Sorry, this car is not available on the given date.", "ok");
      }, () => { this.openSnackBar("Something went wrong", "ok"); });
    } else {
      this.openSnackBar("Please complete all required fields", "ok");
    }
  }
  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 2000,
    });
  }
  clear() {
    this.dates = { from: undefined, to: undefined };
    this.carAvailable = false;
  }
}

interface Car {
  carID: string;
  image: string;
  brand: string;
  productionYear: string;
  color: string;
  doors: string;
  seats: string;
  manualGearbox: boolean;
  airConditioning: boolean;
}
interface Rent {
  carID: string;
  dateFrom: Date;
  dateTo: Date;
}
