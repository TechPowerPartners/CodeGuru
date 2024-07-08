import {ChangeDetectionStrategy, Component} from '@angular/core';
import { TuiAccordionModule } from '@taiga-ui/kit';
@Component({
  selector: 'training-accordion',
  standalone: true,
  imports: [TuiAccordionModule],
  templateUrl: './training.component.html',
  styleUrl: './training.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})

export class TrainingComponent {}

