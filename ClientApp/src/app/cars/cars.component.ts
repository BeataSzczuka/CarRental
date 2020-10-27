import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-cars-component',
  templateUrl: './cars.component.html',
  styleUrls: ['./cars.component.css']
})
export class CarsComponent {
  public cars: Car[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private _snackBar: MatSnackBar) {
    http.get<Car[]>(baseUrl + 'api/car').subscribe(result => {
    this.cars = result;
    }, error => console.error(error));
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
}
