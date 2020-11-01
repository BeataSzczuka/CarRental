import { Component, Inject, ChangeDetectorRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-add-car-component',
  templateUrl: './add-car.component.html'
})
export class AddCarComponent {
  form: FormGroup;
  initialValues = {
    image: undefined,
    brand: '',
    productionYear: undefined,
    color: '',
    doors: undefined,
    seats: undefined,
    manualGearbox: false,
    airConditioning: false
  };
  constructor(private fb: FormBuilder, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private _snackBar: MatSnackBar) {
    this.form = fb.group({
      image: [null, Validators.required],
      brand: ['', Validators.required],
      productionYear: [undefined, Validators.required],
      color: ['', Validators.required],
      doors: [undefined, Validators.required],
      seats: [undefined, Validators.required],
      manualGearbox: [false, Validators.required],
      airConditioning: [false, Validators.required]
    });
  }
  handleFileInput(files: FileList) {
    this.form.patchValue({
      image: files[0]
    });
  }

  onSubmit() {
    if (this.form.valid) {
      const formData = new FormData();
      formData.append('Image', this.form.get('image').value, this.form.get('image').value.name);
      formData.append('Brand', this.form.get('brand').value);
      formData.append('ProductionYear', this.form.get('productionYear').value);
      formData.append('Color', this.form.get('color').value);
      formData.append('Doors', this.form.get('doors').value);
      formData.append('Seats', this.form.get('seats').value);
      formData.append('ManualGearbox', this.form.get('manualGearbox').value);
      formData.append('AirConditioning', this.form.get('airConditioning').value);

      this.http.post<Car>(this.baseUrl + `api/car`, formData).subscribe(() => {
        this.openSnackBar("Car has been successfully added", "ok");
      }, error => { this.openSnackBar(error.error.title, "ok"); });
      this.form.reset(this.initialValues);
    } else {
      this.openSnackBar("Please complete all required fields", "ok");
    }
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
