import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-cars-component',
  templateUrl: './cars.component.html',
  styleUrls: ['./cars.component.scss']
})
export class CarsComponent {
  public cars: Car[] = [];
  public page: number = 1;
  public lastPage: boolean = false;
  today: Date;
  dates = { from: undefined, to: undefined };
  dateFromParam: string = '';
  dateToParam: string = '';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private _snackBar: MatSnackBar, private activatedRoute: ActivatedRoute, private router: Router) {
    this.today = new Date();
    this.activatedRoute.queryParams.subscribe(params => {
      if (!!params['page']) this.page = +params['page'];
      if (!!params['from']) {
        this.dateFromParam = params['from'];
        this.dates.from = new Date(params['from']);
      }
      if (!!params['to']) {
        this.dateToParam = params['to'];
        this.dates.to = new Date(params['to']);
      }
    });
    this.loadCars();
  }
  createImagePath(path: string) {
    path.replace("\\", "/");
    return `https://localhost:44311/${path}`;
  }

  delete(id) {
    this.http.delete<Car>(this.baseUrl + `api/car/${id}`).subscribe(result => {
      this.cars = this.cars.filter((car) => car.carID != id);
      this.openSnackBar("Car has been successfully deleted", "ok");
    }, error => this.openSnackBar("Something went wrong", "ok"));
  }

  loadMore() {
    if (!this.lastPage) {
      this.page++;
      this.router.navigate(['.'], { relativeTo: this.activatedRoute, queryParams: { page: this.page, from: this.dateFromParam, to: this.dateToParam } });
      this.loadCars();
    }
  }


  loadCars() {
    let params = new HttpParams().set('page', this.page.toString());
    if (this.dateFromParam.length > 0) params = params.set('from', this.dateFromParam);
    if (this.dateToParam.length > 0) params = params.set('to', this.dateToParam);

    this.http.get<Car[]>(this.baseUrl + 'api/car', { params }).subscribe(result => {
      this.cars = [ ...this.cars, ...result ];
      if (result.length == 0) this.lastPage = true;
    },
      error => console.error(error)
    );
  }

  filter() {
    this.dateFromParam = this.dates.from;
    this.dateToParam = this.dates.to;
    this.page = 1;
    this.lastPage = false;

    this.router.navigate(['.'], { relativeTo: this.activatedRoute, queryParams: { page: this.page, from: this.dateFromParam, to: this.dateToParam } });
    this.cars = [];
    this.loadCars();
  }
  clear() {
    this.dateFromParam = '';
    this.dateToParam = ''
    this.page = 1;
    this.lastPage = false;
    this.router.navigate(['.'], { relativeTo: this.activatedRoute, queryParams: { page: this.page } });
    this.cars = [];
    this.loadCars();
  }
  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 2000,
    });
  }
}

interface Car {
  carID: string;
  brand: string;
  productionYear: string;
  color: string;
  doors: string;
  seats: string;
  manualGearbox: boolean;
  airConditioning: boolean;
  priceDay: number;
}
